using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeTrackingAPI.Services;
using TimeTrackingAPI.Data;
using TimeTrackingAPI.Models;

namespace TimeTrackingAPI.Services
{
    public class TrackingException : Exception
    {
        public TrackingException(string message) : base(message) { }
    }

    public class Result
    {
        public bool Success { get; set; }
        public Exception Exception { get; set; }
    }

    public class TimeLogService
    {
        private readonly AuthDbContext _context;
        private readonly ILogger<TimeLogService> _logger;

        public TimeLogService(AuthDbContext context, ILogger<TimeLogService> logger)
        {
            _context = context;
            _logger = logger;
        }

        private void CalculateTotalHours(TimeLog log, DateTime nextTime)
        {
            if (log == null) throw new ArgumentNullException(nameof(log), "TimeLog cannot be null.");
            log.TotalHours = nextTime - log.Time;
        }

        private TimeLog GetLastLog(int accountId)
        {
            return _context.TimeLogs
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.Time)
                .FirstOrDefault();
        }

        public Result StartTracking(int accountId)
        {
            var lastLog = GetLastLog(accountId);

            if (lastLog?.Type == TimeLogType.PE)
            {
                _logger.LogWarning("Tracking is already started for AccountId {AccountId}.", accountId);
                return new Result { Success = false, Exception = new TrackingException("Tracking is already started. Cannot start again.") };
            }

            if (lastLog != null && lastLog.Type != TimeLogType.P && lastLog.Type != TimeLogType.PS)
            {
                _logger.LogWarning("Invalid previous state to start tracking for AccountId {AccountId}.", accountId);
                return new Result { Success = false, Exception = new TrackingException("Invalid previous state to start tracking.") };
            }

            if (lastLog != null)
            {
                lastLog.Activ = false;
                CalculateTotalHours(lastLog, DateTime.UtcNow);
                _context.TimeLogs.Update(lastLog);
            }

            var newLog = new TimeLog
            {
                Time = DateTime.UtcNow,
                Type = TimeLogType.PE,
                AccountId = accountId
            };

            _context.TimeLogs.Add(newLog);
            _context.SaveChanges();
            _logger.LogInformation("Tracking started successfully for AccountId {AccountId}.", accountId);

            return new Result { Success = true };
        }

        public Result PauseTracking(int accountId)
        {
            var lastLog = GetLastLog(accountId);

            if (lastLog == null)
            {
                _logger.LogWarning("No active tracking found to pause for AccountId {AccountId}.", accountId);
                return new Result { Success = false, Exception = new TrackingException("No active tracking found to pause.") };
            }

            if (lastLog.Type == TimeLogType.PS)
            {
                _logger.LogWarning("Cannot pause after stopping the session for AccountId {AccountId}.", accountId);
                return new Result { Success = false, Exception = new TrackingException("Cannot pause after stopping the session.") };
            }

            if (lastLog.Type == TimeLogType.P)
            {
                _logger.LogWarning("Session is already paused for AccountId {AccountId}.", accountId);
                return new Result { Success = false, Exception = new TrackingException("Session is already paused.") };
            }

            lastLog.Activ = false;
            CalculateTotalHours(lastLog, DateTime.UtcNow);
            _context.TimeLogs.Update(lastLog);

            var newLog = new TimeLog
            {
                Time = DateTime.UtcNow,
                Type = TimeLogType.P,
                AccountId = accountId
            };

            _context.TimeLogs.Add(newLog);
            _context.SaveChanges();
            _logger.LogInformation("Tracking paused successfully for AccountId {AccountId}.", accountId);

            return new Result { Success = true };
        }

        public Result StopTracking(int accountId)
        {
            var lastLog = GetLastLog(accountId);

            if (lastLog == null || !lastLog.Activ)
            {
                _logger.LogWarning("No active tracking found to stop for AccountId {AccountId}.", accountId);
                return new Result { Success = false, Exception = new TrackingException("No active tracking found to stop.") };
            }

            if (lastLog.Type == TimeLogType.P)
            {
                _logger.LogWarning("Cannot stop while in paused state for AccountId {AccountId}.", accountId);
                return new Result { Success = false, Exception = new TrackingException("Cannot stop while in paused state.") };
            }

            if (lastLog.Type == TimeLogType.PS)
            {
                _logger.LogWarning("Session is already stopped for AccountId {AccountId}.", accountId);
                return new Result { Success = false, Exception = new TrackingException("Session is already stopped.") };
            }

            lastLog.Activ = false;
            CalculateTotalHours(lastLog, DateTime.UtcNow);
            _context.TimeLogs.Update(lastLog);

            var newLog = new TimeLog
            {
                Time = DateTime.UtcNow,
                Type = TimeLogType.PS,
                AccountId = accountId
            };

            _context.TimeLogs.Add(newLog);
            _context.SaveChanges();
            _logger.LogInformation("Tracking stopped successfully for AccountId {AccountId}.", accountId);

            return new Result { Success = true };
        }

        public List<TimeLog> GetLogs(int accountId)
        {
            return _context.TimeLogs
                           .Where(log => log.AccountId == accountId)
                           .OrderBy(log => log.Time)
                           .AsNoTracking()
                           .ToList();
        }



        public Result UpdateLog(Guid logId, DateTime newTime, TimeLogType newType, int? totalHours)
        {
            var log = _context.TimeLogs.FirstOrDefault(t => t.Id == logId);

            if (log == null)
            {
                _logger.LogWarning("TimeLog not found for logId {LogId}.", logId);
                return new Result { Success = false, Exception = new TrackingException("TimeLog not found.") };
            }

            log.Time = newTime;
            log.Type = newType;

            if (totalHours.HasValue)
            {
                log.TotalHours = TimeSpan.FromMinutes(totalHours.Value);
            }

            _context.SaveChanges();
            _logger.LogInformation("TimeLog updated successfully for logId {LogId}.", logId);

            return new Result { Success = true };
        }
    }
}
