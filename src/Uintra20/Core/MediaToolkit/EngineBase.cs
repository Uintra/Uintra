using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading;
using Uintra20.Core.MediaToolkit.Util;

namespace Uintra20.Core.MediaToolkit
{
	public class EngineBase : IDisposable
	{
		private bool isDisposed;
		
		private const string LockName = "MediaToolkit.Engine.LockName";
		
		protected readonly string FFmpegFilePath;
		
		protected readonly Mutex Mutex;
		
		protected Process FFmpegProcess;
		
		protected EngineBase(string ffMpegPath)
		{
			var lockName = $"{LockName}-{ConfigurationManager.AppSettings["Application.Name"]}";
			Mutex = new Mutex(false, lockName);
			isDisposed = false;

			if (ffMpegPath.IsNullOrWhiteSpace())
			{
				throw new FileNotFoundException("<ffmpeg.exe> file not found");
			}

			FFmpegFilePath = ffMpegPath;

			EnsureDirectoryExists();
			EnsureFFmpegFileExists();
			EnsureFFmpegIsNotUsed();
		}

		private void EnsureFFmpegIsNotUsed()
		{
			try
			{
				Mutex.WaitOne();
				Process.GetProcessesByName("ffmpeg")
					   .ForEach(process =>
					   {
						   process.Kill();
						   process.WaitForExit();
					   });
			}
			finally
			{
				Mutex.ReleaseMutex();
			}
		}

		private void EnsureDirectoryExists()
		{
			string directory = Path.GetDirectoryName(FFmpegFilePath) ?? Directory.GetCurrentDirectory(); ;

			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
		}

		private void EnsureFFmpegFileExists()
		{
			if (!File.Exists(FFmpegFilePath))
			{
				UnpackFFmpegExecutable(FFmpegFilePath);
			}
		}

		private static void UnpackFFmpegExecutable(string path)
		{
			Stream compressedFFmpegStream = Assembly.GetExecutingAssembly()
													.GetManifestResourceStream("Uintra20.Core.MediaToolkit.Resources.FFmpeg.exe.gz");

			if (compressedFFmpegStream == null)
			{
				throw new Exception("FFMpeg GZip stream is null");
			}

			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			using (GZipStream compressedStream = new GZipStream(compressedFFmpegStream, CompressionMode.Decompress))
			{
				compressedStream.CopyTo(fileStream);
			}
		}

		public virtual void Dispose()
		{
			Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			if (!disposing || isDisposed)
			{
				return;
			}

			if (FFmpegProcess != null)
			{
				FFmpegProcess.Dispose();
			}
			FFmpegProcess = null;
			isDisposed = true;
		}
	}
}
