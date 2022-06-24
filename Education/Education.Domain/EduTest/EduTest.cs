using Education.Domain.EduTasks;
using System.Text.Json.Serialization;

namespace Education.Domain.EduTests
{
    public class EduTest : EduTask
    {
        public List<EduTestQuestion> Questions { get; set; }
        public EduTestSettings TestSettings { get; set; }
    }

    public class EduTestSettings
    {
        [JsonIgnore]
        public static int AdditionalTime = 10;
        //SECONDS
        public int TimeToSolve { get; set; }
        public int QuestionsAmount { get; set; }
        public bool OneTry { get; set; }
        public bool CalculateScoreOnFinish { get; set; }
        public bool ShowScoreOnFinish { get; set; }
    }
}