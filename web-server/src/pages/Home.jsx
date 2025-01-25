import { useState, useEffect } from 'react';
import Cookies from 'js-cookie';
import AdvertisementCompany from "../components/company/AdvertisementCompany.jsx";
import AdvertisementStudent from "../components/student/AdvertisementStudent.jsx";
const API_SERVER_URL = import.meta.env.VITE_API_SERVER_URL;
function Home() {
    const authData = JSON.parse(Cookies.get('authData'));
    const userType = authData.userType;
    const [adv, setAdv] = useState([]);

    useEffect(() => {
        downloadADV();
    }, []);
    const downloadADV = async () => {
        try {

            const response = await fetch(`${API_SERVER_URL}/api/recommendation/advertisements`, {
                headers: {
                    'Authorization': `Bearer ${authData.token}`,
                },
            });

            if (response.status === 200) {
                const data = await response.json();
                setAdv(data);
            }
        } catch (error) {
            console.error('Error checking profile informations:', error);
        }
    };

    return (
        <>
            {userType.toLowerCase() === 'student' ? (
                <div>
                    <div className="pt-8 px-4 flex justify-center ">
                        <div className="grid grid-cols-2 gap-4 max-w-6xl">
                            {adv.map((advertisement) =>
                                <AdvertisementStudent
                                    key={advertisement.advertisementId}
                                    advertisement={advertisement}
                                />)}
                        </div>
                    </div>
                </div>

            ) : (
                <div className="pt-8 px-4 flex justify-center ">
                    <div className="grid grid-cols-1 gap-4 w-full">
                        {adv.map((advertisement) =>
                            <AdvertisementCompany
                                key={advertisement.advertisementId}
                                advertisement={advertisement}
                            />)}
                    </div>
                </div>

            )}

            <div className="pt-8">
            </div>
        </>

    );
}

export default Home;