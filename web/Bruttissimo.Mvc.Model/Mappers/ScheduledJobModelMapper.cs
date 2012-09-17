using System;
using System.Web;
using Bruttissimo.Common;
using Bruttissimo.Common.Mvc;
using Bruttissimo.Domain;
using Bruttissimo.Domain.Entity;

namespace Bruttissimo.Mvc.Model
{
    public class ScheduledJobModelMapper : IMapperConfigurator
    {
		private readonly IUserService userService;

        public ScheduledJobModelMapper(IUserService userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }
            this.userService = userService;
        }

        public void CreateMaps(IMapper mapper)
        {
            mapper.CreateMap<ScheduledJobDto, ScheduledJobModel>().ForMember(
                m => m.StartTime,
                x => x.MapFrom(j => userService.ToCurrentUserTimeZone(HttpContext.Current.Wrap(), j.StartTime))
            );
        }
    }
}