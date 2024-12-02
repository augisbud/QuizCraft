//import React from "react";
import styles from "./LoadingScreen.module.scss";

export const LoadingScreen = () => {
    return (
        <div className={styles.loadingOverlay}>
            <div className={styles.spinnerContainer}>
                <p className={styles.loadingText}>Uploading your file...</p>
                <div className={styles.spinner}></div>
            </div>
        </div>
    );
};
