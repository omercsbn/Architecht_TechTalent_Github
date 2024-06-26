import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import Paper from '@mui/material/Paper';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import Select from '@mui/material/Select';
import MenuItem from '@mui/material/MenuItem';
import TextField from '@mui/material/TextField';
import { getGoldPrices } from '../../../service/GoldService';
import { getUserAccounts } from '../../../service/User';
import { createGoldAccount } from '../../../service/GoldService';
import Swal from 'sweetalert2';

const IndexPage = () => {
  const [goldPrices, setGoldPrices] = useState({ alis_fiyati: 0, satis_fiyati: 0 });
  const [accounts, setAccounts] = useState([]);
  const [selectedAccount, setSelectedAccount] = useState(null);
  const [isPriceIncreasing, setIsPriceIncreasing] = useState(false);
  const [accountName, setAccountName] = useState('');
  const [amount, setAmount] = useState('');

  useEffect(() => {
    const fetchGoldPrices = async () => {
      try {
        const prices = await getGoldPrices();
        setGoldPrices(prices[0]);
      } catch (error) {
        console.error('Error fetching gold prices:', error);
      }
    };

    const fetchUserAccounts = async () => {
      try {
        const userId = localStorage.getItem('userId');
        const userAccounts = await getUserAccounts(userId);
        const goldAccounts = userAccounts.data.filter(account => account.accountType === 'Deposit' || account.accountType === 'Checking');
        setAccounts(goldAccounts);
      } catch (error) {
        console.error('Error fetching user accounts:', error);
      }
    };

    fetchGoldPrices();
    fetchUserAccounts();
  }, []);

  useEffect(() => {
    if (goldPrices.alis_fiyati > 0 && goldPrices.satis_fiyati > 0) {
      setIsPriceIncreasing(goldPrices.alis_fiyati > goldPrices.satis_fiyati);
    }
  }, [goldPrices]);

  const handleBuyGold = async () => {
    try {
      const numericAmount = parseFloat(amount);
      if (isNaN(numericAmount) || numericAmount <= 0 || numericAmount > selectedAccount.balance) {
        Swal.fire({
          icon: 'error',
          title: 'Hata!',
          text: 'Yetersiz bakiye veya geçersiz miktar!',
        });
        return;
      }

      const data = {
        balance: numericAmount,
        accountName: accountName,
      };
      const response = await createGoldAccount(data);
      if (response.message === "success") {
        Swal.fire({
          icon: 'success',
          title: 'Başarılı!',
          text: 'Altın hesabı başarıyla oluşturuldu.',
        });

        setAccountName('');
        setAmount('');
      } else {
        Swal.fire({
          icon: 'error',
          title: 'Hata!',
          text: response.message || 'Bir hata oluştu.',
        });
      }
    } catch (error) {
      console.error('Error creating gold account:', error);
      Swal.fire({
        icon: 'error',
        title: 'Hata!',
        text: error.message || 'Bir hata oluştu.',
      });
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Paper sx={{ p: 3 }}>
        <Typography variant="h4" gutterBottom>
          Altın Fiyatları
        </Typography>
        <Typography variant="body1" gutterBottom>
          Alış Fiyatı: {goldPrices.alis_fiyati} TL
        </Typography>
        <Typography variant="body1" gutterBottom>
          Satış Fiyatı: {goldPrices.satis_fiyati} TL
        </Typography>
        {isPriceIncreasing ? (
          <Typography variant="body2" color="primary" gutterBottom>
            Altın fiyatları yükseliyor.
          </Typography>
        ) : (
          <Typography variant="body2" color="secondary" gutterBottom>
            Altın fiyatları düşüyor.
          </Typography>
        )}
        <Typography variant="h4" gutterBottom>
          Hesap Seçin
        </Typography>
        <Select
          value={selectedAccount}
          onChange={(event) => setSelectedAccount(event.target.value)}
          fullWidth
          variant="outlined"
        >
          <MenuItem value={null}>Hesap Seçin</MenuItem>
          {accounts.map((account) => (
            <MenuItem key={account.accountID} value={account}>
              {account.accountName} - {account.balance} TL
            </MenuItem>
          ))}
        </Select>
        {selectedAccount && (
          <>
            <TextField
              label="Hesap Adı"
              value={accountName}
              onChange={(event) => setAccountName(event.target.value)}
              fullWidth
              sx={{ mt: 2 }}
            />
            <TextField
              label="Miktar (TL)"
              type="number"
              value={amount}
              onChange={(event) => setAmount(event.target.value)}
              fullWidth
              sx={{ mt: 2 }}
            />
            <Button
              variant="contained"
              onClick={handleBuyGold}
              sx={{ mt: 2 }}
              disabled={!accountName || isNaN(parseFloat(amount)) || parseFloat(amount) <= 0 || !selectedAccount}
            >
              Altın Al
            </Button>
          </>
        )}
      </Paper>
    </Box>
  );
};

export default IndexPage;
