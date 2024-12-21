import { useEffect, useState } from "react";
import styles from "./LoadingScreen.module.scss";

interface LoadingScreenProps {
    progress: number; // Progress percentage (0-100)
}

export const LoadingScreen = ({ progress }: LoadingScreenProps) => {
    const [animatedProgress, setAnimatedProgress] = useState<number>(0);

    useEffect(() => {
        let animationFrameId: number;

        const animateProgress = () => {
            if (animatedProgress < 90) {
                // Quick fill up to 90%
                setAnimatedProgress((prev) => Math.min(prev + 2, 90)); // Adjust the increment for smoothness
            } else if (animatedProgress < 100) {
                // Slow down after 90% to 100%
                const easing = (progress - 90) / 10;
                setAnimatedProgress(90 + easing);
            }

            animationFrameId = requestAnimationFrame(animateProgress);
        };

        animateProgress();

        return () => cancelAnimationFrame(animationFrameId); 
    }, [animatedProgress, progress]);

    return (
        <div className={styles.loadingOverlay}>
            <div className={styles.loadingBarContainer}>
                <div className={styles.loadingTitle}>Creating your quiz...</div>
                <div className={styles.loadingBar}>
                    <div
                        className={styles.progress}
                        style={{ width: `${animatedProgress}%` }}
                    ></div>
                </div>
                <div className={styles.loadingText}>Please wait...</div>
            </div>
        </div>
    );
};
