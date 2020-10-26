until [ $(curl -w '%{http_code}' -s -o /dev/null $1) -eq 200 ]
do
    printf '.'
    sleep 5
done
printf '\nservice responded with HTTP 200\n'