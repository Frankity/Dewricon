﻿namespace DewPlugins
{
    public interface DewPlugins
    {
        string Name { get; }
        string Author { get; }
        string Version { get; }
        void Config();
        void Run();
        void Stop();
    }
}
