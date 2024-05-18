using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Enums;
using NetStone.Common.Extensions;

namespace NetStone.Cache.Db.Resolvers;

public class FreeCompanyFocusDtoResolver : IValueResolver<FreeCompany, FreeCompanyDto, IEnumerable<FreeCompanyFocusDto>>
{
    public IEnumerable<FreeCompanyFocusDto> Resolve(FreeCompany source, FreeCompanyDto destination,
        IEnumerable<FreeCompanyFocusDto> destMember, ResolutionContext context)
    {
        return Enum.GetValues<FreeCompanyFocus>().Cast<Enum>().Where(z =>
                !Equals((int)(object)z, 0) && // filter out empty value
                source.Focus.HasFlag(z))
            .Select(x =>
                new FreeCompanyFocusDto((FreeCompanyFocus)x, x.TryGetDisplayName(out var value) ? value! : x.ToString(),
                    ((FreeCompanyFocus)x).GetFocusIcon()));
    }
}