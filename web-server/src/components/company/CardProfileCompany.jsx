import { useState, useEffect } from 'react';
import Cookies from 'js-cookie';
const API_SERVER_URL = window.env?.VITE_API_SERVER_URL || 'http://localhost:5000';

function CardProfileCompany() {

    const [profileData, setProfileData] = useState({
        username: '',
        email: '',
        password: '',
        bio: '',
        headquarter: '',
        fiscalCode: '',
        vatNumber: ''
    });


    useEffect(() => {
        downloadInformations();
    }, []);

    const downloadInformations = async () => {
        try {
            const authData = JSON.parse(Cookies.get('authData'));
            const response = await fetch(`${API_SERVER_URL}/api/profile/company`, {
                headers: {
                    'Authorization': `Bearer ${authData.token}`,
                },
            });

            if (response.status === 200) {
                const data = await response.json();
                setProfileData(data);

            }
        } catch (error) {
            console.error('Error checking profile informations:', error);
        }
    };


    return (
        <>
            <div className="bg-white flex flex-col rounded-xl p-6 border items-center w-full h-full">
                <div className="flex flex-col pb-4">
                    <img className="w-24 h-24 border rounded-full"
                         src="https://static.vecteezy.com/system/resources/previews/020/911/733/non_2x/profile-icon-avatar-icon-user-icon-person-icon-free-png.png"
                         alt="Profile"/>
                </div>
                <div className="flex flex-col items-center gap">
                    <p className="text-lg font-bold">Username: <span className="font-light">{profileData.username}</span></p>
                    <p className="text-lg font-bold">Email: <span className="font-light">{profileData.email}</span></p>
                    <p className="text-lg font-bold">Head quarter: <span className="font-light">{profileData.headquarter}</span></p>
                    <p className="text-lg font-bold">Fiscal code: <span className="font-light">{profileData.fiscalCode}</span></p>
                    <p className="text-lg font-bold">Vat number: <span className="font-light">{profileData.vatNumber}</span></p>
                    <p className="text-lg font-bold">Your vision: <span className="font-light">{profileData.bio}</span></p>
                </div>
            </div>
        </>
    );
}

export default CardProfileCompany;
