using System;
using System.Collections.Generic;
using System.Linq;
using Fap.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Data.Seed
{
    /// <summary>
    /// Seeds Slots (class sessions) with various test scenarios
    /// </summary>
    public class SlotSeeder : BaseSeeder
    {
        private const int WeeksPerClass = 8;

        private static readonly ClassSchedule DefaultSchedule = new ClassSchedule(
            DayOfWeek.Monday,
            TimeSlotSeeder.Slot1Id,
            DayOfWeek.Wednesday,
            TimeSlotSeeder.Slot1Id);

        private static readonly IReadOnlyDictionary<Guid, ClassSchedule> ClassSchedules = new Dictionary<Guid, ClassSchedule>
        {
            [ClassSeeder.SE101_Winter2025_A] = new ClassSchedule(DayOfWeek.Monday, TimeSlotSeeder.Slot1Id, DayOfWeek.Wednesday, TimeSlotSeeder.Slot1Id),
            [ClassSeeder.CS101_Winter2025_A] = new ClassSchedule(DayOfWeek.Tuesday, TimeSlotSeeder.Slot2Id, DayOfWeek.Thursday, TimeSlotSeeder.Slot2Id),
            [ClassSeeder.MATH101_Winter2025_A] = new ClassSchedule(DayOfWeek.Tuesday, TimeSlotSeeder.Slot3Id, DayOfWeek.Thursday, TimeSlotSeeder.Slot3Id),
            [ClassSeeder.DB201_Winter2025_Evening] = new ClassSchedule(DayOfWeek.Tuesday, TimeSlotSeeder.Slot7Id, DayOfWeek.Friday, TimeSlotSeeder.Slot7Id),

            [ClassSeeder.SE101_Spring2026_A] = new ClassSchedule(DayOfWeek.Monday, TimeSlotSeeder.Slot1Id, DayOfWeek.Wednesday, TimeSlotSeeder.Slot1Id),
            [ClassSeeder.SE102_Spring2026_A] = new ClassSchedule(DayOfWeek.Tuesday, TimeSlotSeeder.Slot4Id, DayOfWeek.Thursday, TimeSlotSeeder.Slot4Id),
            [ClassSeeder.MATH101_Spring2026_A] = new ClassSchedule(DayOfWeek.Monday, TimeSlotSeeder.Slot3Id, DayOfWeek.Thursday, TimeSlotSeeder.Slot3Id),
            [ClassSeeder.CS101_Spring2026_A] = new ClassSchedule(DayOfWeek.Tuesday, TimeSlotSeeder.Slot2Id, DayOfWeek.Thursday, TimeSlotSeeder.Slot2Id),
            [ClassSeeder.CS201_Summer2026_A] = new ClassSchedule(DayOfWeek.Friday, TimeSlotSeeder.Slot5Id, DayOfWeek.Saturday, TimeSlotSeeder.Slot6Id),

            [ClassSeeder.SE101_Fall2026_A] = new ClassSchedule(DayOfWeek.Monday, TimeSlotSeeder.Slot1Id, DayOfWeek.Wednesday, TimeSlotSeeder.Slot1Id),
            [ClassSeeder.SE102_Fall2026_A] = new ClassSchedule(DayOfWeek.Tuesday, TimeSlotSeeder.Slot4Id, DayOfWeek.Thursday, TimeSlotSeeder.Slot4Id),
            [ClassSeeder.DB201_Summer2026_A] = new ClassSchedule(DayOfWeek.Monday, TimeSlotSeeder.Slot5Id, DayOfWeek.Wednesday, TimeSlotSeeder.Slot5Id),
            [ClassSeeder.WEB301_Summer2026_A] = new ClassSchedule(DayOfWeek.Tuesday, TimeSlotSeeder.Slot6Id, DayOfWeek.Thursday, TimeSlotSeeder.Slot6Id),
            [ClassSeeder.WEB301_Fall2026_A] = new ClassSchedule(DayOfWeek.Tuesday, TimeSlotSeeder.Slot6Id, DayOfWeek.Thursday, TimeSlotSeeder.Slot6Id),
            [ClassSeeder.MATH201_Fall2026_A] = new ClassSchedule(DayOfWeek.Monday, TimeSlotSeeder.Slot3Id, DayOfWeek.Wednesday, TimeSlotSeeder.Slot3Id)
        };

        public SlotSeeder(FapDbContext context) : base(context) { }

        public override async Task SeedAsync()
        {
            if (await _context.Slots.AnyAsync())
            {
                Console.WriteLine("Slots already exist. Skipping seeding...");
                return;
            }

            var classes = await _context.Classes
                .Include(c => c.SubjectOffering)
                .ThenInclude(o => o.Semester)
                .ToListAsync();

            if (!classes.Any())
            {
                Console.WriteLine("No classes found. Skipping slot seeding.");
                return;
            }

            var slots = new List<Slot>();

            foreach (var cls in classes)
            {
                var schedule = ClassSchedules.TryGetValue(cls.Id, out var configured)
                    ? configured
                    : DefaultSchedule;

                var semesterStart = cls.SubjectOffering?.Semester?.StartDate.Date ?? DateTime.UtcNow.Date;
                var firstMeeting = AlignToDay(semesterStart, schedule.PrimaryDay);
                var secondMeeting = AlignToDay(semesterStart, schedule.SecondaryDay);

                for (var week = 0; week < WeeksPerClass; week++)
                {
                    var firstDate = firstMeeting.AddDays(week * 7);
                    slots.Add(CreateSlot(cls.Id, firstDate, schedule.PrimarySlotId));

                    var secondDate = secondMeeting.AddDays(week * 7);
                    slots.Add(CreateSlot(cls.Id, secondDate, schedule.SecondarySlotId));
                }
            }

            await _context.Slots.AddRangeAsync(slots);
            await SaveAsync("Slots");

            Console.WriteLine($"Created {slots.Count} slots for {classes.Count} classes over {WeeksPerClass} weeks");
        }

        private static Slot CreateSlot(Guid classId, DateTime date, Guid timeSlotId)
        {
            var status = date.Date < DateTime.UtcNow.Date ? "Completed" : "Scheduled";

            return new Slot
            {
                Id = Guid.NewGuid(),
                ClassId = classId,
                Date = date,
                TimeSlotId = timeSlotId,
                Status = status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        private static DateTime AlignToDay(DateTime startDate, DayOfWeek targetDay)
        {
            var date = startDate;
            while (date.DayOfWeek != targetDay)
            {
                date = date.AddDays(1);
            }

            return date;
        }

        private sealed record ClassSchedule(DayOfWeek PrimaryDay, Guid PrimarySlotId, DayOfWeek SecondaryDay, Guid SecondarySlotId);
    }
}
