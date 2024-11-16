import { GoogleLogin } from "@react-oauth/google";
import { Navbar } from "../../components/Navbar/Navbar";
import styles from "./SignIn.module.scss";
import { useLocation, useNavigate } from "react-router-dom";

export const SignIn = () => {
  const navigate = useNavigate()
  
  const location = useLocation();
  const searchParams = new URLSearchParams(location.search);
  const redirect = searchParams.get("redirect");
  
  return (
    <>
      <Navbar />
      <div className={styles.container}>
        <div className={styles.form}>
          <GoogleLogin
            onSuccess={credentialResponse => {
              if(credentialResponse.credential)
                sessionStorage.setItem("token", credentialResponse.credential);

              navigate(redirect || "/");
            }}
            onError={() => {
              console.error('[GSI] Login Failed');
            }}
          />
        </div>
      </div>
    </>
  );
};
