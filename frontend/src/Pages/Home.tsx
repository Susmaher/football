import type { JSX } from "react";
import { Link } from "react-router-dom";

function Home(): JSX.Element {
    return (
        <>
            <h1>Football wep app</h1>
            <p>
                Ez egy nagyon király weboldal, ahol egy bajnokságot tudsz
                kezelni. Az adatok felvihetők bejelentkezés után
            </p>
            <Link to="/teams">Teams</Link>
            <Link to="/matches">Matches</Link>
            <br />
            <Link to="/admin">Admin</Link>
        </>
    );
}

export default Home;
