import { useEffect, useState } from "react";
import { Hero } from "../../components/Hero/Hero";
import { Navbar } from "../../components/Navbar/Navbar";
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';
import PublicIcon from '@mui/icons-material/Public';
import CircularProgress from '@mui/material/CircularProgress';
import styles from './Home.module.scss';
import { GlobalStatsDto } from '../../types.ts';

export const Home = () => {
    const [stats, setStats] = useState<GlobalStatsDto | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchStats = async () => {
            try {
                const response = await fetch("https://localhost:8080/statistics/global");
                if (!response.ok) {
                    throw new Error("Failed to fetch global statistics");
                }
                const data: GlobalStatsDto = await response.json();
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
            <section className={styles['main-container']}>
                <Stack
                    direction="column"
                    spacing={3}
                    sx={{
                        maxWidth: "500px",
                        width: "100%",
                        alignItems: "center",
                        textAlign: "center",
                    }}
                >
                    {/* Global Statistics Title with Icon */}
                    <Stack direction="row" alignItems="center" spacing={1} className={styles['global-stats-header']}>
                        <Typography variant="h2" sx={{ color: '$text-color' }}>
                            Global Statistics
                        </Typography>
                        <PublicIcon sx={{ fontSize: '3rem', color: '$text-color' }} /> {/* Increase icon size */}
                    </Stack>

                    {/* Loading indicator */}
                    {loading ? (
                        <CircularProgress sx={{ display: 'block', margin: '0 auto' }} />
                    ) : stats ? (
                        <Stack spacing={3} sx={{ width: "100%" }}>
                            <Stack spacing={1} className={styles['stat-item']}>
                                <Typography variant="h6" sx={{ fontWeight: 'bold', fontSize: '1rem' }}>
                                    Total Users
                                </Typography>
                                <Typography variant="body1" sx={{ fontWeight: 'bold', fontSize: '1.5rem' }}>
                                    {stats.totalUsers}
                                </Typography>
                            </Stack>
                            <Stack spacing={1} className={styles['stat-item']}>
                                <Typography variant="h6" sx={{ fontWeight: 'bold', fontSize: '1rem' }}>
                                    Total Quizzes Created
                                </Typography>
                                <Typography variant="body1" sx={{ fontWeight: 'bold', fontSize: '1.5rem' }}>
                                    {stats.totalQuizzesCreated}
                                </Typography>
                            </Stack>
                            <Stack spacing={1} className={styles['stat-item']}>
                                <Typography variant="h6" sx={{ fontWeight: 'bold', fontSize: '1rem' }}>
                                    Average Quizzes Per User
                                </Typography>
                                <Typography variant="body1" sx={{ fontWeight: 'bold', fontSize: '1.5rem' }}>
                                    {stats.averageQuizzesPerUser.toFixed(2)}
                                </Typography>
                            </Stack>
                        </Stack>
                    ) : (
                        <p className={styles['no-stats']}>No statistics available</p>
                    )}
                </Stack>
            </section>
        </main>
    );
};
