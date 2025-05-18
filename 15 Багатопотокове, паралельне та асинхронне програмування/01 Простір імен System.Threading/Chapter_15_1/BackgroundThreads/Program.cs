
using BackgroundThreads;

Printer p = new Printer();

Thread bgroundThread = new Thread(new ThreadStart(p.PrintNumbers));
// This is now a background thread.
//bgroundThread.IsBackground = true;
bgroundThread.Start();
