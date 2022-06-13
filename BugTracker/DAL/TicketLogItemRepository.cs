using BugTracker.DAL;
using BugTracker.Data;
using BugTracker.Models;

namespace BugTracker.DAL {
    public class TicketLogItemRepository : IRepositoryCR<TicketLogItem> {
        private readonly ApplicationDbContext _context;

        public TicketLogItemRepository(ApplicationDbContext context) {
            _context = context;
        }

        public List<TicketLogItem> GetList(Func<TicketLogItem, bool>? whereFunction) {
            if (whereFunction is null) {
                throw new ArgumentNullException();
            }

            var TicketLogItems = _context
                .TicketLogItems
                .Where(whereFunction).ToList();

            return TicketLogItems;
        }

        public TicketLogItem? Get(Func<TicketLogItem, bool>? firstFunction) {
            if (firstFunction is null) {
                throw new ArgumentNullException();
            }

            var TicketLogItem = _context
                .TicketLogItems
                .FirstOrDefault(firstFunction);

            return TicketLogItem;
        }

        public void Create(TicketLogItem? entity) {
            if (entity is null) {
                throw new ArgumentNullException();
            }

            _context.TicketLogItems.Add(entity);
        }

        public void Save() {
            _context.SaveChanges();
        }
    }
}
