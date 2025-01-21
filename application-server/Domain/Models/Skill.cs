public class Skill {

    public int Id { get; set; }
    public string Name { get; set; }

    public Skill(Entity.Skill skill) {
        Id = skill.SkillId;
        Name = skill.Name;
    }

    public Skill(DTO.SkillRegistration skill) {
        Name = skill.Name;
    }

    public DTO.Skill ToDto() {
        return new DTO.Skill {
            Id = Id,
            Name = Name,
        };
    }

    public Entity.Skill ToEntity() {
        return new Entity.Skill {
            SkillId = Id,
            Name = Name,
        };
    }

}
