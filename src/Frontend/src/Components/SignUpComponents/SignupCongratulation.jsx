import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck } from "@fortawesome/free-solid-svg-icons";
import Button from "../../Components/SmallComponents/Button";

import { Link } from "react-router-dom";

const SignupCongratulation = () => {
  const userEmail = localStorage.getItem("registered_email");

  return (
   <div className="pt-850">
      <div className="flex items-center justify-center">
        <div className="flex xl:items-center l:items-center justify-center sm:mt-p md:mt-50p">
          <div className="max-w-md w-full p-6 bg-base-100 rounded-lg shadow-lg flex flex-col items-center">
      <FontAwesomeIcon icon={faCheck} className="text-8xl text-secondary"/>
            <h2 className="mt-8 mb-6">You are successfully registered</h2>
            {userEmail}
            <div className="mt-8 mb-6">
              <ul className="steps">
                <li className="step step-secondary"></li>
                <li className="step step-secondary"></li>
                <li className="step"></li>
              </ul>
            </div>
            <Link to="/login">
              <Button>Login</Button>
            </Link>
          </div>
        </div>
      </div>
      </div>
    
  );
};

export default SignupCongratulation;
