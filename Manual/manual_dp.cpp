// Dipankar Jain
#include <iostream>
using namespace std;

#define MAX 110

int price[MAX], lower[MAX], upper[MAX];

/**
 * Program to generate optimal portfolio from a given basket of securities
 * Input :
 * n : number of stocks
 * p[i]: price of ith stock
 * lower[i]: lower limit of stock in portfolio
 * upper[i]: upper limit of stock in portfolio
 * r: total porftolio value
 *
 * Output :
 * number of valid combinations available
 */

/**
 * Find the total number of valid portfolios
 */
int countSolutions(int n, int r) {
    long long int dp[r+1];                                            // dp[i][j] stores the possible portfolios of j using i
    memset(dp, 0, sizeof(dp));
    dp[0] = 1;
    for(int i=0; i<n; i++) {
        for(int j=price[i]; j<=r; j++) {
            dp[j] += dp[j-price[i]];
        }
        /*
        if(i!=n-1) {
            for(int j=price[i]; j<=r; j++) {
                if(j<lower[i] || j>upper[i]) dp[j] = 0;     // prune the values not required
            }
        }
        */
    }
    return dp[r];
}

/**
 * Runner function
 */
int main()
{
    int n, r;
    cin >> n;                                                   // Number of stocks in basket
    cin >> r;                                                   // Portfolio value
    for(int i=0; i<n; i++) {
        cin >> price[i] >> lower[i] >> upper[i];
        lower[i] = lower[i]*r/100;                              // convert to prices
        upper[i] = upper[i]*r/100;
    }
    cout << countSolutions(n, r) << endl;
    return 0;
}

