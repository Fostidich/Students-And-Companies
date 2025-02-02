import { useState, useEffect } from 'react';
import Cookies from 'js-cookie';
const API_SERVER_URL = window.env?.VITE_API_SERVER_URL || 'http://localhost:5000';

function CardProfileStudent (){
    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [bio, setBio] = useState('');
    const [name, setName] = useState('');
    const [surname, setSurname] = useState('');
    const [university, setUniversity] = useState('');
    const [courseOfStudy, setCourseOfStudy] = useState('');
    const [birthDate, setBirthDate] = useState('');

    useEffect(() => {
        const downloadInformations = async () => {
            try {
                const data = Cookies.get('authData');
                const authData = JSON.parse(data);


                const response = await fetch(`${API_SERVER_URL}/api/profile/student`, {
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
                    setName(data.name);
                    setSurname(data.surname);
                    setUniversity(data.university);
                    setCourseOfStudy(data.courseOfStudy);
                    setBirthDate(data.birthDate.split('T')[0]);

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
                <p className="text-lg font-bold">Name: <span className="font-light">{name}</span></p>
                <p className="text-lg font-bold">Surname: <span className="font-light">{surname}</span></p>
                <p className="text-lg font-bold">University: <span className="font-light">{university}</span></p>
                <p className="text-lg font-bold">Course of study: <span className="font-light">{courseOfStudy}</span></p>
                <p className="text-lg font-bold">Birthdate: <span className="font-light">{birthDate}</span></p>
                <p className="text-lg font-bold">Your bio: <span className="font-light">{bio}</span></p>
            </div>
            <button className="  rounded-xl underline text-white">Edit</button>
        </div>

    );



}
export default CardProfileStudent;
