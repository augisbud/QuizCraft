import { useEffect, useState } from "react";
import { Hero } from "../components/Hero/Hero";
import { Navbar } from "../components/Navbar/Navbar";

interface GlobalStats {
    totalUsers: number;
    totalQuizzesCreated: number;
    averageQuizzesPerUser: number;
}

export const Home = () => {
    const [stats, setStats] = useState<GlobalStats | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchStats = async () => {
            try {
                const response = await fetch("https://localhost:8080/statistics/global");
                if (!response.ok) {
                    throw new Error("Failed to fetch global statistics");
                }
                const data: GlobalStats = await response.json();
                setStats(data);
            } catch (error) {
                console.error("Error fetching global statistics:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchStats();
    }, []);

    return (
        <main>
            <Navbar />
            <Hero />
            <section style={{ padding: "2rem 1rem", marginTop: "2rem" }}>
                <h2 style={{ textAlign: "center", marginBottom: "1.5rem" }}>
                    <span role="img" aria-label="globe" style={{ marginRight: "10px" }}>
                    </span>
                    Global Statistics
                </h2>
                {loading ? (
                    <p style={{ textAlign: "center" }}>Loading...</p>
                ) : stats ? (
                    <div
                        style={{
                            maxWidth: "500px",
                            margin: "0 auto",
                            padding: "2rem",
                            backgroundColor: "#ffffff",
                            borderRadius: "12px",
                            boxShadow: "0 4px 15px rgba(0, 0, 0, 0.1)",
                            textAlign: "center",
                            color: "#333", // Dark color for the text
                        }}
                    >
                        <p style={{ fontSize: "1.2rem", fontWeight: "bold" }}>
                            Total Users: {stats.totalUsers}
                        </p>
                        <p style={{ fontSize: "1.2rem", fontWeight: "bold" }}>
                            Total Quizzes Created: {stats.totalQuizzesCreated}
                        </p>
                        <p style={{ fontSize: "1.2rem", fontWeight: "bold" }}>
                            Average Quizzes Attempted Per User: {stats.averageQuizzesPerUser}
                        </p>
                    </div>
                ) : (
                    <p style={{ textAlign: "center" }}>Error loading statistics</p>
                )}
            </section>
        </main>
    );
};
