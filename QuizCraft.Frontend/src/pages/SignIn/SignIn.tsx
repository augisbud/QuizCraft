import {
  SignInPage,
  type AuthProvider,
  type AuthResponse,
} from "@toolpad/core/SignInPage";
import { Navbar } from "../../components/Navbar/Navbar";
import styles from "./SignIn.module.scss";

const providers = [
  { id: "credentials", name: "Email and password" },
] as AuthProvider[];

const signIn: (
  provider: AuthProvider,
  formData?: FormData
) => Promise<AuthResponse> | void = async (provider, formData) => {
  const promise = new Promise<AuthResponse>((resolve) => {
    setTimeout(() => {
      const email = formData?.get("email");
      const password = formData?.get("password");
      alert(
        `Signing in with "${provider.name}" and credentials: ${email}, ${password}`
      );
      resolve({
        type: "CredentialsSignin",
        error: "Invalid credentials.",
      });
    }, 300);
  });
  return promise;
};

export const SignIn = () => {
  return (
    <>
      <Navbar />
      <div className={styles.container}>
        <div className={styles.form}>
          <SignInPage signIn={signIn} providers={providers} />
        </div>
      </div>
    </>
  );
};
