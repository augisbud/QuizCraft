import styles from "./LoadingScreen.module.scss";

export const LoadingScreen = () => {
    return (
        <div className={styles.loadingOverlay}>
            <div className={styles.loadingBarContainer}>
                <p className={styles.loadingText}>Uploading your file...</p>
                <div className={styles.loadingBar}>
                    <div className={styles.progress}></div>
                </div>
            </div>
        </div>
    );
};
