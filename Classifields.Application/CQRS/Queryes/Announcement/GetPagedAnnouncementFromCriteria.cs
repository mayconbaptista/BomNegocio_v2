using Classifields.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifields.Application.CQRS.Queryes.Announcement
{
    public class GetPagedAnnouncementFromCriteria : IQuery<IEnumerable<AnnouncementEntity>>
    {

    }
}
