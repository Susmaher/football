import type { JSX } from "react";
import { Link } from "react-router-dom";

function SideBar(): JSX.Element {
    return (
        <div
            style={{
                float: "left",
                height: "100vh",
                padding: "1rem",
            }}
        >
            <h3>Navigáció</h3>
            <nav
                style={{
                    display: "flex",
                    flexDirection: "column",
                }}
            >
                <Link to="draw">Sorsolás</Link>
                <Link to="teams">Csapatok koordinálása</Link>
                <Link to="divisions">Osztályok koordinálása</Link>
                <Link to="fields">Pályák koordinálása</Link>
                <Link to="positions">Pozíciók koordinálása</Link>
                <Link to="players">Játékosok koordinálása</Link>
                <Link to="referees">Bírók koordinálása</Link>
            </nav>
        </div>
    );
}

export default SideBar;
