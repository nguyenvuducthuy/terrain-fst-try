﻿using System;
using System.Collections.Concurrent;
using System.Threading;
using OpenTK;
using OpenTK.Audio.OpenAL;
using SimpleShooter.Core.Events;

namespace SimpleShooter.Audio
{
    class SoundManager : IDisposable, ISoundManager
    {
        private object _lockObj;
        private bool _stop;

        private Thread _workThread1;
        private Thread _workThread2;
        private ConcurrentQueue<ShotEventArgs> _shotsQueue;

        public SoundManager()
        {
            _stop = false;
            _lockObj = new object();
            _shotsQueue = new ConcurrentQueue<ShotEventArgs>();
            _workThread1 = new Thread(Play)
            {
                IsBackground = true
            };
            _workThread2 = new Thread(Play)
            {
                IsBackground = true
            };

            if (Config.IsSoundOn)
            {
                _workThread1.Start();
            }

            //_workThread2.Start();
        }

        public void Shot(ShotEventArgs args)
        {
            if (Config.IsSoundOn)
            {
                _shotsQueue.Enqueue(args);
            }
        }

        public void Dispose()
        {
            _stop = true;
        }

        private void Play()
        {
            while (true)
            {

                ShotEventArgs args;
                if (_shotsQueue.TryDequeue(out args))
                {
                    PlayShot(args);
                }
                lock (_lockObj)
                {
                    if (_stop)
                    {
                        break;
                    }
                }
            }
        }



        private unsafe void PlayShot(ShotEventArgs args)
        {
            var device = Alc.OpenDevice(null);
            var ctx = Alc.CreateContext(device, (int*)null);
            Alc.MakeContextCurrent(ctx);

            var buffer = AL.GenBuffer();
            var source = AL.GenSource();

            int sampleFreq = 44100;
            double dt = 2 * Math.PI / sampleFreq;
            double amp = 0.5;

            int freq = 440;
            var dataCount = sampleFreq / freq;

            var sinData = new short[dataCount];
            for (int i = 0; i < sinData.Length; ++i)
            {
                sinData[i] = (short)(amp * short.MaxValue * Math.Sin(i * dt * freq));
            }
            AL.BufferData(buffer, ALFormat.Mono16, sinData, sinData.Length, sampleFreq);
            AL.Source(source, ALSourcei.Buffer, buffer);
            AL.Source(source, ALSourceb.Looping, true);

            AL.SourcePlay(source);
            Thread.Sleep(100);
            
            if (ctx != ContextHandle.Zero)
            {
                Alc.MakeContextCurrent(ContextHandle.Zero);
                Alc.DestroyContext(ctx);
            }
            ctx = ContextHandle.Zero;

            if (device != IntPtr.Zero)
            {
                Alc.CloseDevice(device);
            }
            device = IntPtr.Zero;
        }

    }
}
