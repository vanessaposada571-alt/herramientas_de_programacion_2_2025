using Hospital.domain.model;

namespace Hospital.domain.ports
{
    public interface Medical_insurance_port
    {
        Medical_insurance FindByPolicy_number(Medical_insurance insurance);
        Medical_insurance FindByIdSure(Medical_insurance insurance);
        void Save(Medical_insurance insurance);
        void Update(Medical_insurance insurance);
    }
}
