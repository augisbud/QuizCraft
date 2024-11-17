const options: Intl.DateTimeFormatOptions = {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
    hour12: false,
};

export const formatDate = (date: Date) => {
    return date.toLocaleString('lt-lt', options).replace(',', '');
};
