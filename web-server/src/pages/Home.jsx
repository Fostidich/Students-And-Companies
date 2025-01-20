import CardJobHome from "../components/CardJobHome.jsx";

// eslint-disable-next-line react/prop-types
function Home({ jobs }) {
    return (
        <>
            <div className="pt-8 px-4 flex justify-center ">
                <div className="grid grid-cols-3 gap-8 max-w-6xl">
                    {/* eslint-disable-next-line react/prop-types */}
                    {jobs.map((job) =>
                        <CardJobHome
                            key={job.id}
                            title={job.title}
                            imgURL={job.imgURL}
                            jobPosition={job.jobPosition}
                        />)}
                </div>
            </div>
            <div className="pt-8">
            </div>
        </>

    );
}

export default Home;