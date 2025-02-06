namespace JollyRoger.DarkHeresy.Data
{
    [System.Serializable]
	public class AdvancementData
    {
        public string name = "";
        public int xp = 0;
        public int lvl = -1;
        public Skill.Type type;

        public AdvancementData(Advancement advancement)
        {
            name = advancement.name;
            xp = advancement.xp;
            if(advancement.type != Advancement.Type.Talent)
                lvl = advancement.mastery;
        }
    }
}