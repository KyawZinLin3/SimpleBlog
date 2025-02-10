using SimpleBlog.WebAPI.Entites;
using SimpleBlog.WebAPI.Repositories.Base;
using System.Data.SqlTypes;

namespace SimpleBlog.WebAPI.Services
{
    public class TagService
    {
        private readonly IRepository<Tag> _tagRepository;
        public TagService(IRepository<Tag> tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<Tag>> GetAllTags() => await _tagRepository.GetAllAsync();

        public async Task<Tag> GetTagById(int Id)
        {
            var tag = await _tagRepository.GetByIdAsync(Id);
            if (tag == null) throw new NullReferenceException("Tag is not found");
            return tag;
        }

        public async Task<bool> CreateTag(Tag tag)
        {
            await _tagRepository.AddAsync(tag);
            var changes = await _tagRepository.SaveChangesAsync();
            if (changes > 0)
                return true;

            return false;

        }

        public async Task<bool> UpdateTag(Tag tag)
        {
            _tagRepository.Update(tag);
            var changes = await _tagRepository.SaveChangesAsync();
            if (changes > 0)
                return true;

            return false;
        }

        public async Task<bool> DeleteTag(int Id)
        {
            var tag = await _tagRepository.GetByIdAsync(Id);
            if (tag == null) throw new NullReferenceException("Tag is not found");

            _tagRepository.Delete(tag);
            var changes = await _tagRepository.SaveChangesAsync();
            if (changes > 0)
                return true;

            return false;
        }
    }
}
