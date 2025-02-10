using Microsoft.Identity.Client;
using SimpleBlog.WebAPI.Entites;
using SimpleBlog.WebAPI.Repositories.Base;

namespace SimpleBlog.WebAPI.Services
{
    public class MediaService
    {
        private readonly IRepository<Media> _mediaRepository;
        public MediaService(IRepository<Media> mediaRepository)
        {
            _mediaRepository = mediaRepository;
        }

        public async Task<IEnumerable<Media>> GetAllMedias() => await _mediaRepository.GetAllAsync();

        public async Task<Media> GetMediaById(int Id)
        {
            var result = await _mediaRepository.GetByIdAsync(Id);
            if (result == null) throw new NullReferenceException("Media not found.");
            return result;
        }

        public async Task<bool> CreateMedia(Media media)
        {
            await _mediaRepository.AddAsync(media);
            var changes = await _mediaRepository.SaveChangesAsync();
            if(changes>0) return true;
            return false;
        }

        public async Task<bool> UpdateMedia(Media media)
        {
            _mediaRepository.Update(media);
            var changes = await _mediaRepository.SaveChangesAsync();
            if(changes>0) return true;
            return false;
        }

        public async Task<bool> DeleteMedia(int Id)
        {
            var tag = await _mediaRepository.GetByIdAsync(Id);
            if (tag == null) throw new NullReferenceException("Tag is not found");

            _mediaRepository.Delete(tag);
            var changes = await _mediaRepository.SaveChangesAsync();
            if (changes > 0)
                return true;

            return false;
        }
    }
}
