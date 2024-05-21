const Home = () => {
    return (
        <section className="m-0 p-0 text-center">
            <img
                src="/medicalAppointmentLogo.jpeg"
                alt="Medical Appointment Logo"
                className="w-full max-w-lg mx-auto border-4 border-blue-500 rounded-lg"
            />
            <h1 className="text-3xl font-bold mt-4">Medical Appointment System</h1>

            <div className="p-4 bg-gray-50 flex flex-col items-center w-3/4 lg:w-1/2 mx-auto">
                <div className="text-left p-2 text-lg">
                    <p className="mb-6">
                        This is <b>Raphael Torres Cordon</b>&apos;s course completion project<br/>
                        From the Postgraduate course: .NET Systems Architecture with Azure<br/>
                        By FIAP - Faculty of Informatics and Administration Paulista
                    </p>

                    <p className="">
                        Esse é o projeto de conclusão de curso de <b>Raphael Torres Cordon</b><br/>
                        Do curso de Pós Tech: Arquitetura de Sistemas .NET com Azure<br/>
                        Pela FIAP - Faculdade de Informática e Administração Paulista
                    </p>
                </div>
            </div>
        </section>
    );
};

export default Home;
