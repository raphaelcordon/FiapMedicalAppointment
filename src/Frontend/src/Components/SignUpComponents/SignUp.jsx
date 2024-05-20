import Button from "../../Components/SmallComponents/Button";
import { useEffect, useRef, useState } from "react";
import { useNavigate } from "react-router-dom";
import { RegisterNewUser } from "../../axios/account.js";

const SignUp = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [passwordRepeat, setPasswordRepeat] = useState('');
  const [role, setRole] = useState('');
  const [address, setAddress] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');

  const [isToggled, setIsToggled] = useState(null);
  const navigate = useNavigate();
  const [error, setError] = useState('');

  const handleSignUp = async (e) => {
    e.preventDefault();
    setError('');

    // Check if passwords match
    if (password !== passwordRepeat) {
      setError('Passwords do not match.');
      return;
    }

    const userData = { email, password, role, address, phoneNumber };

    try {
      setError('');
      // Registration
      await RegisterNewUser(userData);
      navigate("/login");
    } catch (error) {
      if (error.response && error.response.data && error.response.data.message) {
        setError(error.response.data.message);
      } else {
        setError("This email is already registered");
      }
      console.log(error);
    }
  };

  return (
    <div className="flex xl:items-center l:items-center justify-center sm:mt-50 md:mt-50">
      <div className="max-w-md w-full p-6 bg-base-100 rounded-lg shadow-lg">
        <form onSubmit={handleSignUp}>
          <div className="mb-4">
            <div className="mb-2">
              <label htmlFor="email" className="block text-center mb-2 text-sm text-accent-content">
                Enter your e-mail
              </label>
              <input
                  type="email"
                  id="email"
                  name="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  placeholder="name@email.com"
                  className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-secondary text-center"
                  required
              />
            </div>

            <div className="mb-2">
              <label className="block mb-2 text-sm text-accent-content">Password<span
                  className="text-red-500">*</span></label>
              <input
                  name="password"
                  id="password"
                  type="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  placeholder="6 char and capital letters"
                  className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-secondary"
                  required
              />
            </div>
            <div className="mb-2">
              <label className="block mb-2 text-sm text-accent-content">Confirm Password<span
                  className="text-red-500">*</span></label>
              <input
                  name="passwordRepeat"
                  id="passwordRepeat"
                  type="password"
                  value={passwordRepeat}
                  onChange={(e) => setPasswordRepeat(e.target.value)}
                  className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-secondary"
                  required
              />
            </div>

            <div className="mb-2">
              <label className="block mb-2 text-sm text-accent-content">Address<span
                  className="text-red-500">*</span></label>
              <input
                  name="address"
                  id="address"
                  type="text"
                  value={address}
                  onChange={(e) => setAddress(e.target.value)}
                  className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-secondary"
                  required
              />
            </div>

            <div className="mb-2">
              <label className="block mb-2 text-sm text-accent-content">Phone Number<span
                  className="text-red-500">*</span></label>
              <input
                  name="phoneNumber"
                  id="phoneNumber"
                  type="tel"
                  value={phoneNumber}
                  onChange={(e) => setPhoneNumber(e.target.value)}
                  className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-secondary"
                  required
              />
            </div>

            <div className="mb-2">
              <label className="block mb-2 text-sm text-accent-content">Role<span
                  className="text-red-500">*</span></label>
              <select
                  name="role"
                  id="role"
                  value={role}
                  onChange={(e) => setRole(e.target.value)}
                  className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-secondary"
                  required
              >
                <option value="" disabled>- - -</option>
                <option value="admin">Admin</option>
                <option value="doctor">Doctor</option>
                <option value="patient">Patient</option>
              </select>
            </div>

            <Button onClick={handleSignUp} disabled=
                {
                    email === "" ||
                    password === "" ||
                    passwordRepeat === "" ||
                    address === "" ||
                    phoneNumber === "" ||
                    role === ""
                }
            >Register</Button>
            {error && <p className="text-error text-sm mt-2">{error}</p>}
          </div>
        </form>
      </div>
    </div>
  );
};

export default SignUp;
