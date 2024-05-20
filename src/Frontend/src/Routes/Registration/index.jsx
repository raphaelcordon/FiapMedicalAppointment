import SignUp from "../../Components/SignUpComponents/SignUp.jsx";

const Registration = () => {

    return (
        <>
            <div className="flex items-center justify-center h-full">
                <div className="max-w-md w-full p-6 bg-base-100 rounded-lg shadow-lg">
                    <h1 className="text-2xl font-semibold text-center mt-4 mb-2">Account creation</h1>
                    <div className="flex justify-around mb-6">
                        <SignUp />
                    </div>

                </div>
            </div>
        </>
    )
}
export default Registration;