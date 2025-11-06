using Hospital.domain.model;

namespace Hospital.domain.ports
{
 public interface Medical_insurance_port
 {
 // Buscar póliza por número de póliza (se recibe un objeto con Policy_number)
 Medical_insurance FindByPolicy_number(Medical_insurance insurance);

 // Buscar por IdSure si aplica en dominio
 Medical_insurance FindByIdSure(Medical_insurance insurance);

 // Persistencia
 void Save(Medical_insurance insurance);
 void Update(Medical_insurance insurance);
 void Delete(Medical_insurance insurance);
 }
}
