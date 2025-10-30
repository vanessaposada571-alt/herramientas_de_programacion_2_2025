using Hospital.domain.model;
using Hospital.domain.services;

namespace Hospital.application.usecases
{
    public class HumanResourcesUseCase
    {
        private readonly C_Employee _createEmployee;
        private readonly U_Employee _updateEmployee;
        private readonly S_Employee _selectEmployee;

        public HumanResourcesUseCase(
            C_Employee createEmployee,
            U_Employee updateEmployee,
            S_Employee selectEmployee)
        {
            _createEmployee = createEmployee;
            _updateEmployee = updateEmployee;
            _selectEmployee = selectEmployee;
        }

        public void Register(User user)
        {
            _createEmployee.Create(user);
        }

        public void Update(User user)
        {
            _updateEmployee.Update(user);
        }

        public User Select(User user)
        {
            return _selectEmployee.Select(user);
        }
    }
}