using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Uintra20.Core.MediaToolkit.Model;
using Uintra20.Core.MediaToolkit.Options;
using Uintra20.Core.MediaToolkit.Util;

namespace Uintra20.Core.MediaToolkit
{
    public class Engine : EngineBase
    {
        public event EventHandler<ConversionCompleteEventArgs> ConversionCompleteEvent;

        public Engine(string ffMpegPath) : base(ffMpegPath)
        {
        }

        public void Convert(MediaFile inputFile, MediaFile outputFile, ConversionOptions options)
        {
            EngineParameters engineParams = new EngineParameters
            {
                InputFile = inputFile,
                OutputFile = outputFile,
                ConversionOptions = options,
                Task = FFmpegTask.Convert
            };

            FFmpegEngine(engineParams);
        }
        
        public void Convert(MediaFile inputFile, MediaFile outputFile)
        {
            EngineParameters engineParams = new EngineParameters
            {
                InputFile = inputFile,
                OutputFile = outputFile,
                Task = FFmpegTask.Convert
            };

            FFmpegEngine(engineParams);
        }

        public event EventHandler<ConvertProgressEventArgs> ConvertProgressEvent;

        public void CustomCommand(string ffmpegCommand)
        {
            if (ffmpegCommand.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("ffmpegCommand");
            }

            EngineParameters engineParameters = new EngineParameters {CustomArguments = ffmpegCommand};

            StartFFmpegProcess(engineParameters);
        }
        
        public void GetMetadata(MediaFile inputFile)
        {
            EngineParameters engineParams = new EngineParameters
            {
                InputFile = inputFile,
                Task = FFmpegTask.GetMetaData
            };

            FFmpegEngine(engineParams);
        }
        
        public void GetThumbnail(MediaFile inputFile, MediaFile outputFile, ConversionOptions options)
        {
            EngineParameters engineParams = new EngineParameters
            {
                InputFile = inputFile,
                OutputFile = outputFile,
                ConversionOptions = options,
                Task = FFmpegTask.GetThumbnail
            };

            FFmpegEngine(engineParams);
        }

        #region Private method - Helpers

        private void FFmpegEngine(EngineParameters engineParameters)
        {
            if (!engineParameters.InputFile.Filename.StartsWith("http://") &&
                !File.Exists(engineParameters.InputFile.Filename))
            {
                throw new FileNotFoundException("Input file not found", engineParameters.InputFile.Filename);
            }

            try
            {
                Mutex.WaitOne();
                StartFFmpegProcess(engineParameters);
            }
            finally
            {
                Mutex.ReleaseMutex();
            }
        }

        private ProcessStartInfo GenerateStartInfo(EngineParameters engineParameters)
        {
            string arguments = CommandBuilder.Serialize(engineParameters);

            return GenerateStartInfo(arguments);
        }

        private ProcessStartInfo GenerateStartInfo(string arguments)
        {
            //windows case
            if (Path.DirectorySeparatorChar == '\\')
            {
                return new ProcessStartInfo
                {
                    Arguments = "-nostdin -y -loglevel info " + arguments,
                    FileName = FFmpegFilePath,
                    CreateNoWindow = true,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
            }

            return new ProcessStartInfo
            {
                Arguments = "-y -loglevel info " + arguments,
                FileName = FFmpegFilePath,
                CreateNoWindow = true,
                RedirectStandardInput = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };
        }

        #endregion

        private void OnConversionComplete(ConversionCompleteEventArgs e)
        {
            EventHandler<ConversionCompleteEventArgs> handler = ConversionCompleteEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

      
        private void OnProgressChanged(ConvertProgressEventArgs e)
        {
            EventHandler<ConvertProgressEventArgs> handler = ConvertProgressEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void StartFFmpegProcess(EngineParameters engineParameters)
        {
            List<string> receivedMessagesLog = new List<string>();
            TimeSpan totalMediaDuration = new TimeSpan();

            ProcessStartInfo processStartInfo = engineParameters.HasCustomArguments
                ? GenerateStartInfo(engineParameters.CustomArguments)
                : GenerateStartInfo(engineParameters);

            using (FFmpegProcess = Process.Start(processStartInfo))
            {
                Exception caughtException = null;
                if (FFmpegProcess == null)
                {
                    throw new InvalidOperationException("FFmpeg process is not running.");
                }

                FFmpegProcess.ErrorDataReceived += (sender, received) =>
                {
                    if (received.Data == null) return;
#if (DebugToConsole)
                    Console.WriteLine(received.Data);
#endif
                    try
                    {
                        receivedMessagesLog.Insert(0, received.Data);
                        if (engineParameters.InputFile != null)
                        {
                            RegexEngine.TestVideo(received.Data, engineParameters);
                            RegexEngine.TestAudio(received.Data, engineParameters);

                            Match matchDuration = RegexEngine.Index[RegexEngine.Find.Duration].Match(received.Data);
                            if (matchDuration.Success)
                            {
                                if (engineParameters.InputFile.Metadata == null)
                                {
                                    engineParameters.InputFile.Metadata = new Metadata();
                                }

                                TimeSpan.TryParse(matchDuration.Groups[1].Value, out totalMediaDuration);
                                engineParameters.InputFile.Metadata.Duration = totalMediaDuration;
                            }
                        }

                        ConversionCompleteEventArgs convertCompleteEvent;
                        ConvertProgressEventArgs progressEvent;

                        if (RegexEngine.IsProgressData(received.Data, out progressEvent))
                        {
                            progressEvent.TotalDuration = totalMediaDuration;
                            OnProgressChanged(progressEvent);
                        }
                        else if (RegexEngine.IsConvertCompleteData(received.Data, out convertCompleteEvent))
                        {
                            convertCompleteEvent.TotalDuration = totalMediaDuration;
                            OnConversionComplete(convertCompleteEvent);
                        }
                    }
                    catch (Exception ex)
                    {
                        // catch the exception and kill the process since we're in a faulted state
                        caughtException = ex;

                        try
                        {
                            FFmpegProcess.Kill();
                        }
                        catch (InvalidOperationException)
                        {
                            // swallow exceptions that are thrown when killing the process, 
                            // one possible candidate is the application ending naturally before we get a chance to kill it
                        }
                    }
                };

                FFmpegProcess.BeginErrorReadLine();
                FFmpegProcess.WaitForExit();

                if ((FFmpegProcess.ExitCode != 0 && FFmpegProcess.ExitCode != 1) || caughtException != null)
                {
                    throw new Exception(
                        FFmpegProcess.ExitCode + ": " + receivedMessagesLog[1] + receivedMessagesLog[0],
                        caughtException);
                }
            }
        }
    }
}