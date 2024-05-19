import { useSelector } from "react-redux";
import UserDataUpdate from "../../Components/UserDataUpdateComponent/index.jsx";

const Patient = () => {
  const user = useSelector((state) => state.user.userData);

  return <UserDataUpdate user={user} />;
};

export default Patient;