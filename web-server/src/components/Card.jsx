import 'react';

const Card = ({title, imgURL, jobPosition}) => {
    return (
        <div className="w-80 h-96 bg-white rounded-md overflow-hidden border border-gray-300 flex flex-col items-start">
            {/* Image Section */}
            <div className="">
                <img src={imgURL} alt="" className="w-[350px] h-[192px]" />
            </div>

            {/* Content Section */}
            <div className="w-full h-full p-6 flex flex-col justify-start items-start gap-6 border">
                {/* Company and Role */}
                <div className="w-full h-[70px] flex flex-col justify-start items-start gap-2">
                    <div className="w-[302px] text-gray-800 text-2xl font-bold break-words">{title}</div>
                    <div className="w-[302px] text-gray-800 text-base font-normal underline leading-7 break-words">{jobPosition}</div>
                </div>

                {/* Apply Button */}
                <div className="w-[100px] h-[50px] relative bg-gray-800 rounded-md">
                    <div className="absolute inset-0 bg-gray-800 rounded-md" />
                    <button className="absolute left-[27px] top-[15px] text-center text-white text-base font-bold break-words">Apply</button>
                </div>
            </div>
        </div>
    );
};

export default Card;
