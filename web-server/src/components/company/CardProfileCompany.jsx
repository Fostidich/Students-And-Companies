import { useState, useEffect } from 'react';
import Cookies from 'js-cookie';
const API_SERVER_URL = import.meta.env.VITE_API_SERVER_URL;
function CardProfileCompany (){
    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [bio, setBio] = useState('');
    const [headquarter, setHeadquarter] = useState('');
    const [fiscalCode, setFiscalCode] = useState('');
    const [vatNumber, setVatNumber] = useState('');

    useEffect(() => {
        const downloadInformations = async () => {
            try {
                const authData = JSON.parse(Cookies.get('authData'));


                const response = await fetch(`${API_SERVER_URL}/api/profile/company`, {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${authData.token}`, // Invia il token nell'header Authorization
                    },
                });

                if (response.status === 200){
                    const data = await response.json();

                    setUsername(data.username);
                    setEmail(data.email);
                    setBio(data.bio);
                    setHeadquarter(data.headquarter);
                    setFiscalCode(data.fiscalCode);
                    setVatNumber(data.vatNumber);


                }else
                    console.error('Error checking profile informations:', response.status);

            } catch (error) {
                console.error('Error checking profile informations:', error);
            }
        };

        downloadInformations();
    }, []);

    return (

        <div className="bg-white flex flex-col rounded-xl p-6 border items-center">
            <div className="flex flex-col pb-4">
                <img className="w-24 h-24 border rounded-full"
                     src={"https://static.vecteezy.com/system/resources/previews/020/911/733/non_2x/profile-icon-avatar-icon-user-icon-person-icon-free-png.png"}/>
            </div>
            <div className="flex flex-col items-center gap">
                <p className="text-lg font-bold">Username: <span className="font-light">{username}</span></p>
                <p className="text-lg font-bold">Email: <span className="font-light">{email}</span></p>
                <p className="text-lg font-bold">Head quarter: <span className="font-light">{headquarter}</span></p>
                <p className="text-lg font-bold">Fiscal code: <span className="font-light">{fiscalCode}</span></p>
                <p className="text-lg font-bold">Vat number: <span className="font-light">{vatNumber}</span></p>
                <p className="text-lg font-bold">Your vision: <span className="font-light">{bio}</span></p>
            </div>
            <button className=" text-white rounded-xl underline text-blue-500">Edit</button>
        </div>

    );



}
export default CardProfileCompany;