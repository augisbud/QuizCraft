import { GoogleLogin } from "@react-oauth/google";
import { Navbar } from "../../components/Navbar/Navbar";
import styles from "./SignIn.module.scss";
import { useLocation, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";

export const SignIn = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const searchParams = new URLSearchParams(location.search);
    const redirect = searchParams.get("redirect");

    // State to check if user is signed in
    const [isSignedIn, setIsSignedIn] = useState(false);

    useEffect(() => {
        // Check if there's a token in sessionStorage when the component mounts
        const token = sessionStorage.getItem("token");
        if (token) {
            setIsSignedIn(true); // User is signed in
        }
    }, []);

    const handleSignOut = () => {
        // Clear the sessionStorage token and update the state
        sessionStorage.removeItem("token");
        setIsSignedIn(false);
    };

    return (
        <>
            <Navbar />
            <div className={styles.formContainer}>
                <div className={styles.form}>
                    {/* Render Sign In text only if the user is not signed in */}
                    {!isSignedIn && (
                        <h2 className={styles.title}>Sign In to Your Account</h2>
                    )}

                    {/* Show Google Login button if the user is not signed in */}
                    {!isSignedIn ? (
                        <div className={styles.googleButtonContainer}> {/* Added a wrapper for the className */}
                            <GoogleLogin
                                onSuccess={credentialResponse => {
                                    if (credentialResponse.credential) {
                                        // Save the token in sessionStorage
                                        sessionStorage.setItem("token", credentialResponse.credential);
                                        setIsSignedIn(true); // Update state to reflect the sign-in
                                        navigate(redirect || "/"); // Redirect to the previous page or homepage
                                    }
                                }}
                                onError={() => {
                                    console.error("[GSI] Login Failed");
                                }}
                                theme="outline"
                                shape="circle"
                            />
                        </div>
                    ) : (
                        <>
                            {/* Display "You are signed in" and the Sign Out button when signed in */}
                                <h1 className={styles.title}>You are signed in!</h1>
                            <button onClick={handleSignOut} className={styles.signOutButton}>
                                Sign Out
                            </button>
                        </>
                    )}
                </div>
            </div>
        </>
    );
};
