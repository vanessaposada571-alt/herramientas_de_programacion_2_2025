using Hospital.domain.model;

namespace Hospital.application.ports
{
    public interface IHumanResourcesPort
    {
        User FindById(User user);
        User FindByName(User user);
        void Save(User user);
        void Update(User user);
        void Delete(User user);
        User Search(User user);
    }
}