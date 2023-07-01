namespace dotnet_rpg.Models;

public class LevelExperienceThreshold
{
    public int Level { get; set; }
    public long ExperienceThreshold { get; set; }

    public static List<LevelExperienceThreshold> GetAll()
    {
        return new List<LevelExperienceThreshold>() {
            new LevelExperienceThreshold {Level = 1, ExperienceThreshold = 100 },
            new LevelExperienceThreshold {Level = 2, ExperienceThreshold = 210 },
            new LevelExperienceThreshold {Level = 3, ExperienceThreshold = 331 },
            new LevelExperienceThreshold {Level = 4, ExperienceThreshold = 464 },
            new LevelExperienceThreshold {Level = 5, ExperienceThreshold = 610 },
            new LevelExperienceThreshold {Level = 6, ExperienceThreshold = 771 },
            new LevelExperienceThreshold {Level = 7, ExperienceThreshold = 948 },
            new LevelExperienceThreshold {Level = 8, ExperienceThreshold = 1143},
            new LevelExperienceThreshold {Level = 9, ExperienceThreshold = 1357},
            new LevelExperienceThreshold {Level = 10,  ExperienceThreshold = 1593 },
            new LevelExperienceThreshold {Level = 11,  ExperienceThreshold = 1852 },
            new LevelExperienceThreshold {Level = 12,  ExperienceThreshold = 2137 },
            new LevelExperienceThreshold {Level = 13,  ExperienceThreshold = 2450 },
            new LevelExperienceThreshold {Level = 14,  ExperienceThreshold = 2795 },
            new LevelExperienceThreshold {Level = 15,  ExperienceThreshold = 3174 },
            new LevelExperienceThreshold {Level = 16,  ExperienceThreshold = 3587 },
            new LevelExperienceThreshold {Level = 17,  ExperienceThreshold = 4036 },
            new LevelExperienceThreshold {Level = 18,  ExperienceThreshold = 4521 },
            new LevelExperienceThreshold {Level = 19,  ExperienceThreshold = 5046 },
            new LevelExperienceThreshold {Level = 20,  ExperienceThreshold = 5616 },
            new LevelExperienceThreshold {Level = 21,  ExperienceThreshold = 6231 },
            new LevelExperienceThreshold {Level = 22,  ExperienceThreshold = 6895 },
            new LevelExperienceThreshold {Level = 23,  ExperienceThreshold = 7611 },
            new LevelExperienceThreshold {Level = 24,  ExperienceThreshold = 8380 },
            new LevelExperienceThreshold {Level = 25,  ExperienceThreshold = 9203 },
            new LevelExperienceThreshold {Level = 26, ExperienceThreshold = 10085 },
            new LevelExperienceThreshold {Level = 27, ExperienceThreshold = 11094 },
            new LevelExperienceThreshold {Level = 28, ExperienceThreshold = 12168 },
            new LevelExperienceThreshold {Level = 29, ExperienceThreshold = 13309 },
            new LevelExperienceThreshold {Level = 30, ExperienceThreshold = 14516 },
            new LevelExperienceThreshold {Level = 31, ExperienceThreshold = 15789 },
            new LevelExperienceThreshold {Level = 32, ExperienceThreshold = 17126 },
            new LevelExperienceThreshold {Level = 33, ExperienceThreshold = 18535 },
            new LevelExperienceThreshold {Level = 34, ExperienceThreshold = 20014 },
            new LevelExperienceThreshold {Level = 35, ExperienceThreshold = 21565 },
            new LevelExperienceThreshold {Level = 36, ExperienceThreshold = 23189 },
            new LevelExperienceThreshold {Level = 37, ExperienceThreshold = 24886 },
            new LevelExperienceThreshold {Level = 38, ExperienceThreshold = 26656 },
            new LevelExperienceThreshold {Level = 39, ExperienceThreshold = 28500 },
            new LevelExperienceThreshold {Level = 40, ExperienceThreshold = 30419 },
            new LevelExperienceThreshold {Level = 41, ExperienceThreshold = 32420 },
            new LevelExperienceThreshold {Level = 42, ExperienceThreshold = 34509 },
            new LevelExperienceThreshold {Level = 43, ExperienceThreshold = 36684 },
            new LevelExperienceThreshold {Level = 44, ExperienceThreshold = 38947 },
            new LevelExperienceThreshold {Level = 45, ExperienceThreshold = 41309 },
            new LevelExperienceThreshold {Level = 46, ExperienceThreshold = 43789 },
            new LevelExperienceThreshold {Level = 47, ExperienceThreshold = 46397 },
            new LevelExperienceThreshold {Level = 48, ExperienceThreshold = 49146 },
            new LevelExperienceThreshold {Level = 49, ExperienceThreshold = 52036 },
            new LevelExperienceThreshold {Level = 50, ExperienceThreshold = 55095 }
        };
    }

    public static bool IsLevelUp(long startingXpTotal, long newXpTotal)
    {
        var allThresholds = GetAll();

        foreach (var threshold in allThresholds)
        {
            if (startingXpTotal < threshold.ExperienceThreshold && newXpTotal >= threshold.ExperienceThreshold)
            {
                return true;
            }
        }

        return false;
    }
}
