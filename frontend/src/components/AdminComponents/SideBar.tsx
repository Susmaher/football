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
            </nav>
        </div>
    );
}

export default SideBar;
