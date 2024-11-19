import Button from '@mui/material/Button';
import { styled } from '@mui/material/styles';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import IconButton from '@mui/material/IconButton';
import CloseIcon from '@mui/icons-material/Close';
import { useReactiveVar } from '../../utils/useReactiveVar';
import { individualAnalyticsDialogVar } from '../../utils/Cache';
import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from '@mui/material';
import { useEffect, useState } from 'react';
import { QuizAttemptsDto } from '../../utils/QuizCraftAPIClient';
import { client } from '../../utils/Client';
import { formatDate } from '../../utils/Date';

const BootstrapDialog = styled(Dialog)(({ theme }) => ({
    '& .MuiDialogContent-root': {
        padding: theme.spacing(2),
    },
    '& .MuiDialogActions-root': {
        padding: theme.spacing(1),
    },
}));

export const IndividualAnalytics = () => {
    const individualAnalyticsDialog = useReactiveVar(individualAnalyticsDialogVar);

    const [quizAttempts, setQuizAttempts] = useState<QuizAttemptsDto | null>(null);

    useEffect(() => {
        const fetchQuizAttempts = async () => {
            const data = await client.quizzesGET(individualAnalyticsDialog!);

            setQuizAttempts(data);
        };

        if (individualAnalyticsDialog)
            fetchQuizAttempts();
    }, [individualAnalyticsDialog]);

    return (
        <BootstrapDialog
            fullWidth={true}
            maxWidth="sm"
            onClose={() => individualAnalyticsDialogVar.set(null)}
            aria-labelledby="customized-dialog-title"
            open={individualAnalyticsDialog != null}
        >
            <DialogTitle sx={{ m: 0, p: 2 }} id="customized-dialog-title">
                Results for <b>{quizAttempts?.name}</b>
            </DialogTitle>
            <IconButton
                aria-label="close"
                onClick={() => individualAnalyticsDialogVar.set(null)}
                sx={(theme) => ({
                    position: 'absolute',
                    right: 8,
                    top: 8,
                    color: theme.palette.grey[500],
                })}
            >
                <CloseIcon />
            </IconButton>
            <DialogContent dividers>
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Attempted At</TableCell>
                                <TableCell>Correct / Total Questions</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {quizAttempts?.attempts.map((row, index) => (
                                <TableRow
                                    key={index}
                                    sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
                                >
                                    <TableCell>{ formatDate(row.startedAt) }</TableCell>
                                    <TableCell>{ row.correctAnswers } / {quizAttempts?.answers}</TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </DialogContent>
            <DialogActions>
                <Button autoFocus onClick={() => individualAnalyticsDialogVar.set(null)}>
                    Close
                </Button>
            </DialogActions>
        </BootstrapDialog>
    )
}