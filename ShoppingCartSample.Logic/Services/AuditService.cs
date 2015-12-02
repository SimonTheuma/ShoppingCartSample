using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Data.Repositories;
using ShoppingCartSample.Domain.Enums;
using ShoppingCartSample.Domain.Models;

namespace ShoppingCartSample.Logic.Services
{
    public class AuditService : IAuditService
    {
        private IAuditRepository _auditRepository { get; set; }

        public AuditService(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public void LogUserAction(UserActionBase action)
        {
            _auditRepository.LogUserAction(action);
        }
    }
}
