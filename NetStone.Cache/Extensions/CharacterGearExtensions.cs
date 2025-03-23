using NetStone.Cache.Db.Models;
using NetStone.Common.Extensions;

namespace NetStone.Cache.Extensions;

internal static class CharacterGearExtensions
{
    public static List<string> GetMateriaList(this CharacterGear characterGear)
    {
        var list = new List<string>();
        list.AddIfNotNull(characterGear.Materia1);
        list.AddIfNotNull(characterGear.Materia2);
        list.AddIfNotNull(characterGear.Materia3);
        list.AddIfNotNull(characterGear.Materia4);
        list.AddIfNotNull(characterGear.Materia5);
        return list;
    }
}