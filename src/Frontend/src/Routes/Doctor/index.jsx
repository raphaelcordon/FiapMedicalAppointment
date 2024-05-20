import { useSelector } from "react-redux";
import UserDataUpdate from "../../Components/UserDataUpdateComponent/index.jsx";
import ListDoctorMedicalSpecialties from "../../Components/MedicalSpecialtyComponent/ListDoctorMedicalSpecialties.jsx";

const Doctor = () => {
  const user = useSelector((state) => state.user.userData);
  const medicalSpecialty = useSelector((state) => state.medicalSpecialty.medicalSpecialtyData);


  return <>
    <ListDoctorMedicalSpecialties user={user} medicalSpecialty={medicalSpecialty} />
    <UserDataUpdate user={user} />;
  </>

};

export default Doctor;
