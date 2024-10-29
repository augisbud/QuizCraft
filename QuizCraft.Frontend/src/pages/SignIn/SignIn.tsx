import { GoogleLogin } from "@react-oauth/google";
import { Navbar } from "../../components/Navbar/Navbar";
import styles from "./SignIn.module.scss";

export const SignIn = () => {
  return (
    <>
      <Navbar />
      <div className={styles.container}>
        <div className={styles.form}>
          <GoogleLogin
            onSuccess={credentialResponse => {
              if(credentialResponse.credential)
                sessionStorage.setItem("token", credentialResponse.credential);
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
