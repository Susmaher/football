import type { JSX } from "react";

function SideBar(): JSX.Element {
    return (
        <div style={{ float: "left", height: "100vh", padding: "1rem" }}>
            <h3>Navigáció</h3>
            <p>Drawing</p>
        </div>
    );
}

export default SideBar;
