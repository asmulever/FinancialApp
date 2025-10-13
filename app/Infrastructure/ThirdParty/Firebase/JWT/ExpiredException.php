<?php

namespace Finhub\Infrastructure\ThirdParty\Firebase\JWT;

class ExpiredException extends \UnexpectedValueException
{
    private object $payload;

    private ?int $timestamp = null;

    public function setPayload(object $payload): void
    {
        $this->payload = $payload;
    }

    public function getPayload(): object
    {
        return $this->payload;
    }

    public function setTimestamp(int $timestamp): void
    {
        $this->timestamp = $timestamp;
    }

    public function getTimestamp(): ?int
    {
        return $this->timestamp;
    }
}